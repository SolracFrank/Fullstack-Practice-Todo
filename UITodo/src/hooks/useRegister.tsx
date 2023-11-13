import { useCallback } from "react";
import { registerService } from "../services/registerService";
import { Register } from "../interfaces/interface";

export const useRegister = () => {
  const registerUser = useCallback(async (data: Register) => {
    const response = await registerService(data);

    return response;
  }, []);

  return { registerUser };
};

export default useRegister;
