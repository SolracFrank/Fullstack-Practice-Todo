import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useAuthContext } from "../../../context/useAuthContext";
import {
  PutTodo,
  TodoInformation,
  filteredDataSetup,
} from "../../../interfaces/interface";
import { useState } from "react";
import { useUpdateTodoState } from "../../../hooks/useTodo";
import { AxiosError } from "axios";
import toast from "react-hot-toast";

interface UpdateStateProps {
  state: string;
  idTodo: number;
}
type MutationContext = {
  previousTodo: TodoInformation;
};
const UpdateState = ({ state, idTodo }: UpdateStateProps) => {
  const { todoUser } = useAuthContext();
  const { updateTodo } = useUpdateTodoState();
  const queryKey = ["todos"];
  const queryClient = useQueryClient();
  const [todoState, setTodoState] = useState<PutTodo>({
    idTodo: idTodo,
    idUser: todoUser!,
    estado: state,
  });

  const mutation = useMutation(updateTodo, {
    onMutate: async (newTodo) => {
      await queryClient.cancelQueries(queryKey);
      const previousTodo =
        queryClient.getQueryData<TodoInformation[]>(queryKey);
      if (previousTodo) {
        const todoIndex = previousTodo.findIndex(
          (t) => t.idTodo === newTodo.idTodo
        );

        if (todoIndex !== -1) {
          const updatedTodos = [...previousTodo];

          updatedTodos[todoIndex] = {
            ...updatedTodos[todoIndex],
            estado: newTodo.estado,
          };

          queryClient.setQueryData(queryKey, updatedTodos);
        }
      }

      return { previousTodo, newTodo };
    },
    onError: (_error: AxiosError, _variables, context: MutationContext) => {
      queryClient.setQueryData(queryKey, context.previousTodo);

      toast.error("Error al insertar: " + _error.message);
      if (
        _error.response &&
        _error.response.data &&
        (_error.response.status >= 400 || _error.response.status < 500)
      ) {
        toast.error("Hay un error en la petición ", _error.response.data);
      } else {
        toast.error(`Ocurrió un error desconocido`);
      }
    },
    onSettled: () => {
      queryClient.invalidateQueries(queryKey);
    },
  });

  async function HandleClick() {
    const newTodoState = {
      ...todoState,
      estado:
        todoState.estado === filteredDataSetup.Active
          ? filteredDataSetup.Completed
          : filteredDataSetup.Active,
    };

    setTodoState(newTodoState);

    await mutation.mutate(newTodoState);
  }

  return (
    <div
      className={`px-2 py-1 w-1/3 rounded-full cursor-pointer text-center  ${
        state == filteredDataSetup.Active
          ? "bg-teal-100 active:bg-white dark:bg-calid-300 dark:active:bg-calid-100"
          : "bg-day-400 active:bg-day-200 dark:bg-calid-400 dark:active:bg-calid-350"
      }`}
      onClick={HandleClick}
    >
      {state}
    </div>
  );
};

export default UpdateState;
