import { QueryKey, useMutation, useQueryClient } from "@tanstack/react-query";
import { useAuthContext } from "../../../context/useAuthContext";
import { useDeleteCompleted } from "../../../hooks/useTodo";
import {
  TodoInformation,
  filteredDataSetup,
} from "../../../interfaces/interface";
import { AxiosError } from "axios";
import toast from "react-hot-toast";

type MutationContext = {
  previousTodos: TodoInformation[];
};

const DeleteCompletedButton = () => {
  const { deleteCompleted } = useDeleteCompleted();
  const { todoUser } = useAuthContext();
  const queryKey: QueryKey = ["todos"];
  const queryClient = useQueryClient();

  const mutation = useMutation(deleteCompleted, {
    onMutate: async () => {
      await queryClient.cancelQueries(queryKey);

      const previousTodos =
        queryClient.getQueryData<TodoInformation[]>(queryKey);

      if (previousTodos)
        queryClient.setQueryData<TodoInformation[]>(
          queryKey,
          previousTodos.filter(
            (todo) => todo.estado !== filteredDataSetup.Completed
          )
        );

      return { previousTodos };
    },
    onError: (_error: AxiosError, _variables, context: MutationContext) => {
      queryClient.setQueryData(queryKey, context.previousTodos);
      toast.error("Error al eliminar: " + _error.message);
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

  if (!todoUser) return <h1>Error de autentificación</h1>;
  async function HandleClick() {
    await mutation.mutate(todoUser!);
  }
  return (
    <button
      onClick={HandleClick}
      className="hover:text-oscure-200 active:text-calid-300"
    >
      Delete completed
    </button>
  );
};

export default DeleteCompletedButton;
