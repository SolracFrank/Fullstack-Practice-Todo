import { ZodType, z } from "zod";
import { AddTodo } from "../interfaces/interface";


const addTodoSchema: ZodType<AddTodo> = z.object({
  description: z.string().max(255, { message: "El limite es 255 caracteres" }),
  dateLimit: z.string().refine(
    (dateString) => {
      const date = new Date(dateString);
      return !isNaN(date.getTime());
    },
    { message: "La fecha no tiene el formato correcto" }
  ),
  // state: z.enum([State.Active, State.Completed])
});

export default addTodoSchema;
