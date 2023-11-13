import { ZodType, z } from "zod";
import { Login } from "../interfaces/interface";

const LoginSchema: ZodType<Login> = z.object({
  email: z.string().email({ message: "Email is not valid" }),
  password: z.string().min(1, { message: "Field cannot be empty" }),
});

export default LoginSchema;