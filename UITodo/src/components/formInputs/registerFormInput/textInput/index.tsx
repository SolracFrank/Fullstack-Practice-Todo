import { ReactNode } from "react";
import { inputRegisterFormStyles } from "../../../../styles/formStyles";
import { FieldErrors, UseFormRegister } from "react-hook-form";
import { Register } from "../../../../interfaces/interface";

type FieldNames =
  | "name"
  | "lastName"
  | "email"
  | "username"
  | "password"
  | "confirmPassword";

interface TextInputProps {
  register: UseFormRegister<Register>;
  errors: FieldErrors<Register>;
  fieldName: FieldNames;
  type?: "text" | "email"
}

export const TextInput = ({ register, errors, fieldName, type = "text" }: TextInputProps) => {
  return (
    <section className={inputRegisterFormStyles.section}>
      <label htmlFor={fieldName} className={inputRegisterFormStyles.label}>
        {fieldName.replace(/\s+/g, '').toUpperCase()}
      </label>
      <input
        id={fieldName.replace(/\s+/g, '')}
        type={type}
        className={inputRegisterFormStyles.input}
        placeholder={fieldName.replace(/\s+/g, '').charAt(0).toLocaleUpperCase() + fieldName.slice(1).toLocaleLowerCase()}
        {...register(fieldName)}
      />
    {errors[fieldName]?.message && (
    <p className={inputRegisterFormStyles.zodMessageLabel}>
        {errors[fieldName]!.message as ReactNode}
    </p>
)}
    </section>
  );
};
