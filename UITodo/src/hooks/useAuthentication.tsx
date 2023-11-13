import { useCallback } from "react";
import { loginService } from "../services/authenticationService";
import { Login, UserCookies } from "../interfaces/interface";
import api from "../services/axiosService.ts";
import Cookies from "universal-cookie";
import { getUserCookies, setUserCookies } from "../helper/cookieSetter.ts";

export const useAuthenticate = () => {
  const loginUser = useCallback(async (data: Login) => {
    const response = await loginService(data);
    return response;
  }, []);

  return { loginUser };
};

export const refreshTokenService = async (cookies: Cookies) => {
  const {
    refreshToken,
    userId,
    jwToken: oldJwtToken,
  } = getUserCookies(cookies);

  const refresh = { userId, refreshToken, oldJwtToken };

  const response = await api.post("/authorization/refreshjwt", refresh);

  const newUserCookies: UserCookies = {
    cookies: cookies,
    userId: response.data.id,
    userName: response.data.userName,
    jwToken: response.data.jwToken,
    refreshToken: response.data.refreshToken,
    jwtExpires: response.data.jwtExpires,
    refreshTokenExpires: response.data.refreshTokenExpires,
    todoUser: response.data.userId,
  };

  console.table (newUserCookies);
  setUserCookies(newUserCookies);

  return response;
};

export default useAuthenticate;
