import { Login } from "../interfaces/interface.ts";
import api from "./axiosService.ts";

export const loginService = async (loginData: Login) => {
  const response = await api.post("/authorization/login", loginData);
  return response;
};