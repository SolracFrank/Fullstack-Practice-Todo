import { ZodType, z } from "zod";
import { Register } from "../interfaces/interface";

const RegisterSchema: ZodType<Register> = z.object({
  name: z
    .string()
    .min(1, { message: "Name cannot be empty" })
    .max(50, { message: "Max name length is 50" }),
  lastName: z
    .string()
    .min(1, { message: "Last name cannot be empty" })
    .max(50, { message: "Max Last name length is 50" }),
  username: z
    .string()
    .min(1, { message: "Username cannot be empty" })
    .max(50, { message: "Max Username length is 50" }),
  email: z.string().email({ message: "Email is not valid" }),
  password: z.string().regex(/^(?=.*[A-Z])(?=.*[!@#$_%^&*]).{6,15}$/, {
    message:
      "Password must be 6-15 characters length and must have at least one uppercase letter and one special character",
  }),
  confirmPassword: z.string()
     .min(1, { message: "Field cannot be empty" })
  ,
}).refine(data => data.password === data.confirmPassword, {
  message: "Passwords don't match",
  path: ['confirmPassword']
});

export default RegisterSchema;
