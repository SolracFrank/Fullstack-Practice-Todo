import { DateTime } from "luxon";
import Cookies from "universal-cookie";
import { UserCookies } from "../interfaces/interface";

export const setUserCookies = ({
  cookies,
  jwtExpires,
  userId,
  userName,
  jwToken,
  refreshToken,
  refreshTokenExpires,
  todoUser
}: UserCookies) => {

  const jwtExpriesPlus = DateTime.fromISO(jwtExpires, { zone: "utc" })
    .plus({ minutes: 2 })
    .toJSDate();
  cookies.set("userId", userId, {
    expires: new Date(refreshTokenExpires),
  });
  cookies.set("userName", userName, {
    expires: new Date(refreshTokenExpires),
  });
  cookies.set("JWToken", jwToken, { 
    expires: new Date(refreshTokenExpires),
  });
  cookies.set("JWTokenExpires", jwtExpriesPlus, {
    expires: new Date(refreshTokenExpires),
  });
  cookies.set("RefreshToken", refreshToken, {
    expires: new Date(refreshTokenExpires),
  });
  cookies.set("RefreshTokenExpires", refreshTokenExpires, {
    expires: new Date(refreshTokenExpires),
  });
  cookies.set("todoUser", todoUser, {
    expires: new Date(refreshTokenExpires),
  });
};

export const removeUserCookies = (cookies: Cookies) => {
  cookies.remove("userId");
  cookies.remove("userName");
  cookies.remove("JWToken");
  cookies.remove("JWTokenExpires");
  cookies.remove("RefreshToken");
  cookies.remove("RefreshTokenExpires");
  cookies.remove("todoUser");
};

export const getUserCookies = (cookies: Cookies) => {
  const userName = cookies.get("userName");
  const refreshToken = cookies.get("RefreshToken");
  const userId = cookies.get("userId");
  const jwToken = cookies.get("JWToken");
  const todoUser = cookies.get("todoUser");


  const jwtExpiresRaw = cookies.get("JWTokenExpires");
  const jwtExpiresDecoded = jwtExpiresRaw ? decodeURIComponent(jwtExpiresRaw) : "";

  const refreshTokenExpiresRaw = cookies.get("RefreshTokenExpires");
  const refreshTokenExpiresDecoded = refreshTokenExpiresRaw ? decodeURIComponent(refreshTokenExpiresRaw) : "";

  return {
    userName,
    refreshToken,
    userId,
    jwToken,
    jwtExpires: jwtExpiresDecoded,
    refreshTokenExpires: refreshTokenExpiresDecoded,
    todoUser
  };
};

