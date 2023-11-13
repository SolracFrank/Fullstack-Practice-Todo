import axios, { AxiosError } from "axios";
import { ProblemDetails } from "../interfaces/interface";
import toast from "react-hot-toast";
import Cookies from "universal-cookie";
import {
  getUserCookies,
  removeUserCookies,
  setUserCookies,
} from "../helper/cookieSetter";

let isRefreshingToken = false;
type SubscriberCallback = (token: string) => void;

let subscribers: SubscriberCallback[] = [];

function addSubscriber(callback: SubscriberCallback) {
  subscribers.push(callback);
  console.log("Suscriptores: ", subscribers);
}

function onRefreshed(pepe: string): void {
  subscribers.map((callback) => callback(pepe));
  subscribers = [];
}

const api = axios.create({
  baseURL: import.meta.env.VITE_API_ENDPOINT,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use(
  async (config) => {
    const cookies = new Cookies();
    const { jwToken: token } = getUserCookies(cookies);

    config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => {
    Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    const axiosError = error as AxiosError;
    const originalRequest = error.config;

    const cookies = new Cookies();

    if (axiosError.code === "ERR_NETWORK") toast.error("Connection error");

    if (axiosError.response?.status === 500) {
      const problemDetail = axiosError.response?.data as ProblemDetails;
      toast.error(`${problemDetail?.detail}`);
    }

    if (error.response.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      const {
        refreshToken,
        userId,
        jwToken: oldJwtToken,
        refreshTokenExpires,
      } = getUserCookies(cookies);

      console.log("Intentando refrescar...");

      const refreshTokenExpiry = new Date(refreshTokenExpires).getTime();

      if (Date.now() > refreshTokenExpiry) {
        subscribers = [];
        toast.error("Por favor, vuelve a iniciar sesión.");
        removeUserCookies(cookies);
        window.location.href = "/login";
        return Promise.reject(error);
      }

      if (!isRefreshingToken) {
        isRefreshingToken = true;

        try {
          const res = await api.post("/authorization/refreshjwt", {
            userId,
            refreshToken,
            oldJwtToken,
          });
          const newToken = res.data.jwToken;
          await setUserCookies({
            cookies: cookies,
            jwtExpires: res.data.jwtExpires,
            userId: userId,
            userName: res.data.userName,
            jwToken: newToken,
            refreshToken: res.data.refreshToken,
            refreshTokenExpires: res.data.refreshTokenExpires,
            todoUser: res.data.userId,
          });
       
          originalRequest.headers["Authorization"] =
            "Bearer " + newToken;
          isRefreshingToken = false;
          onRefreshed(newToken);

          return api(originalRequest);
        } catch (err) {
          subscribers = [];
          isRefreshingToken = false;
          removeUserCookies(cookies);
          toast.error("Ocurrió un problema con la sesión.");
        }
      }

      return new Promise((resolve) => {
        addSubscriber((token) => {
          originalRequest.headers["Authorization"] = "Bearer " + token;
          resolve(api(originalRequest));
        });
      });
    }
    return Promise.reject(error);
  }
);

export default {
  get: api.get,
  post: api.post,
  put: api.put,
  delete: api.delete,
  patch: api.patch,
};
