import React, {
  createContext,
  useCallback,
  useEffect,
  useMemo,
  useRef,
  useState,
} from "react";
import Cookies from "universal-cookie";
import {
  AuthenticationContext,
  AuthenticationContextType,
  IAuthProvider,
} from "../interfaces/authContextInterface";
import {
  getUserCookies,
  removeUserCookies,
  setUserCookies,
} from "../helper/cookieSetter";
import { refreshTokenService } from "../hooks/useAuthentication";

export const AuthContext = createContext<AuthenticationContextType | undefined>(
  undefined
);

export const AuthProvider: React.FC<IAuthProvider> = ({ children }) => {
  const cookies = useMemo(() => new Cookies(), []);
  const [isRefreshing, setIsRefreshing] = useState(false);

  const isUserAuthenticated = useCallback(
    () => !!cookies.get("JWToken"),
    [cookies]
  );

  const [isAuthenticated, setIsAuthenticated] = useState(isUserAuthenticated());

  const [userName, setUserName] = useState<string | null>(
    cookies.get("userName")
  );
  const [todoUser, setTodoUser] = useState<number | null>(
    cookies.get("todoUser")
  );

  const login = ({
    userId,
    userName,
    jwToken,
    refreshToken,
    jwtExpires,
    refreshTokenExpires,
    todoUser,
  }: AuthenticationContext) => {
    setUserCookies({
      cookies,
      jwtExpires,
      jwToken,
      refreshToken,
      refreshTokenExpires,
      userId,
      userName,
      todoUser,
    });
    setIsAuthenticated(true);
    setUserName(userName || null);
    setTodoUser(todoUser || null);
  };

  const logout = useCallback(() => {
    removeUserCookies(cookies);
    setIsAuthenticated(false);
    setUserName(null);
    setTodoUser(null);
  }, [cookies]);

  const hasSetInterval = useRef(false);

  useEffect(() => {
    const {
      jwToken,
      userName: userNameCookie,
      todoUser,
    } = getUserCookies(cookies);
    setIsAuthenticated(!!jwToken);
    setUserName(userNameCookie || null);
    setTodoUser(todoUser || null);

    const checkAndRefreshToken = async () => {
      setIsRefreshing(true);
      const jwtTokenExpiry = new Date(cookies.get("JWTokenExpires")).getTime();
      const renewThreshold = 5 * 60 * 1000;
      if (Date.now() >= jwtTokenExpiry - renewThreshold) {
        try {
          if (cookies.get("RefreshToken")) {
            await refreshTokenService(cookies);
          }
        } catch (error) {
          console.error("Error al refrescar el token:", error);
        } finally {
          setIsRefreshing(false);
        }
      }
    };

    let intervalId: number;
    if (isUserAuthenticated() && !hasSetInterval.current) {
      hasSetInterval.current = true;
      intervalId = setInterval(checkAndRefreshToken, 1 * 60 * 1000);
    }

    return () => {
      if (intervalId) {
        clearInterval(intervalId);
      }
    };
  }, [cookies, isUserAuthenticated, logout]);

  useEffect(() => {
    if (!isAuthenticated && !isRefreshing) {
      logout();
    }
  }, [logout, isAuthenticated, isRefreshing]);

  const contextValue = {
    isAuthenticated,
    login,
    logout,
    userName,
    todoUser,
  };

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};
