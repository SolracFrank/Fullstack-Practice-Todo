import { Register } from "../interfaces/interface.ts";
import api from "./axiosService.ts";

export const registerService = async (registerData: Register) => {
  const response = await api.post("/authorization/register", registerData);
  return response;
};
