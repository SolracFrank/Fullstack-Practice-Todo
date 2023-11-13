import { ReactNode } from "react";
import { inputLoginFormStyles } from "../../../../styles/formStyles";
import { FieldErrors, UseFormRegister } from "react-hook-form";
import { Login } from "../../../../interfaces/interface";

type FieldNames =
  | "password"
  | "email"


interface TextInputProps {
  register: UseFormRegister<Login>;
  errors: FieldErrors<Login>;
  fieldName: FieldNames;
  type?: "text" | "email"
}

export const TextInput = ({ register, errors, fieldName, type = "text" }: TextInputProps) => {
  return (
    <section className={inputLoginFormStyles.section}>
      <label htmlFor={fieldName} className={inputLoginFormStyles.label}>
        {fieldName.replace(/\s+/g, '').toUpperCase()}
      </label>
      <input
        id={fieldName.replace(/\s+/g, '')}
        type={type}
        className={inputLoginFormStyles.input}
        placeholder={fieldName.replace(/\s+/g, '').charAt(0).toLocaleUpperCase() + fieldName.slice(1).toLocaleLowerCase()}
        {...register(fieldName)}
      />
    {errors[fieldName]?.message && (
    <p className={inputLoginFormStyles.zodMessageLabel}>
        {errors[fieldName]!.message as ReactNode}
    </p>
)}
    </section>
  );
};
