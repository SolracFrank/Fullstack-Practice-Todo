import { FieldErrors, UseFormRegister } from "react-hook-form";
import { AddTodo } from "../../../../interfaces/interface";
import { ReactNode } from "react";

type types = "datetime-local";

interface SelectableInputProps {
  errors: FieldErrors<AddTodo>;
  register: UseFormRegister<AddTodo>;
  type: types;
}

const SelectableInput = ({ errors, register, type }: SelectableInputProps) => {
  return (
    <div className="mb-4 w-full ">
      {type == "datetime-local" ? (
        <>
          <label className="block text-xl font-bold mb-2 text-center ">
            Fecha Límite
          </label>
          <input
            type="datetime-local"
            className="shadow appearance-none border rounded-3xl w-full py-2 px-3  leading-tight focus:shadow-outline transition duration-150
        bg-gray-100 dark:bg-oscure-400 text-gray-800 dark:text-gray-300  border-calid-400 dark:border-calid-200 focus:border-calid-350 dark:focus:border-calid-100"
            {...register("dateLimit")}
          />
          {errors["dateLimit"]?.message && (
            <p className="mt-2 text-start text-sm font-light text-red-600">
              {errors["dateLimit"]!.message as ReactNode}
            </p>
          )}
        </>
      ) : (
        <div>
          <p>error</p>
        </div>
      )}
    </div>
  );
};
export default SelectableInput;
