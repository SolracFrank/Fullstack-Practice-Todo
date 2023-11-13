import { FieldErrors, UseFormRegister } from "react-hook-form";
import { AddTodo } from "../../../../interfaces/interface";
import { ReactNode } from "react";

type fieldNames = "description";

interface TextInputProps {
  errors: FieldErrors<AddTodo>;
  register: UseFormRegister<AddTodo>;
  fieldName: fieldNames;
  placeholder: string;
}

const TextInput = ({
  errors,
  register,
  fieldName,
  placeholder,
}: TextInputProps) => {
  return (
    <>
      <label className="block text-xl font-bold mb-2 text-center">
        Descripción
      </label>
      <input
        type="text"
        placeholder={placeholder}
        autoComplete="off"
        className="shadow appearance-none border rounded-3xl w-full py-2 px-3 
         leading-tight  focus:shadow-outline transition duration-150
         bg-gray-100 dark:bg-oscure-400 text-gray-800 dark:text-gray-300  border-calid-400 dark:border-calid-200 focus:border-calid-350 dark:focus:border-calid-100"
        {...register(fieldName)}
      />
      {errors[fieldName]?.message && (
        <p className="mt-2 text-start text-sm font-light text-red-600">
          {errors[fieldName]!.message as ReactNode}
        </p>
      )}
    </>
  );
};
export default TextInput;
