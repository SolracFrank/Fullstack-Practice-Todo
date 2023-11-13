import { DateTime } from "luxon";
import { TodoInformation } from "../../../interfaces/interface";
import UpdateState from "./updateStateTodo";
import DeleteTodoButton from "./deleteTodoButton";
import { forwardRef } from "react";

interface TodoProps {
  todo: TodoInformation;
  dateTime: DateTime;
  todoUser: number | null;
}
const Todo = forwardRef<HTMLLIElement, TodoProps>(
  ({ todo, dateTime, todoUser, ...props }: TodoProps, ref) => {
    return (
      <li
        {...props}
        ref={ref}
        className="p-2 bg-white dark:bg-oscure-300 grid grid-cols-3 gap-x-2 rounded-lg hover:bg-oscure-500 transition duration-200 items-center"
      >
        <div className="col-span-1">{todo.description}</div>
        <div className="col-span-1 flex flex-col items-start gap-y-1">
          <span className="font-bold">LÍMITE</span>
          {dateTime
            .setZone("local")
            .setLocale("es")
            .toFormat("dd 'de' MMMM 'del' yyyy 'a las' hh:mm a")}
        </div>
        <div className="col-span-1 flex justify-between">
          <UpdateState state={todo.estado} idTodo={todo.idTodo} />
          <DeleteTodoButton todoId={todo.idTodo} userId={todoUser!} />
        </div>
      </li>
    );
  }
);

export default Todo;
