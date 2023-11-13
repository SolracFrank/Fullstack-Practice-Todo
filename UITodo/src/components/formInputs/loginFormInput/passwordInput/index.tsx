import { FieldErrors, UseFormRegister } from "react-hook-form";
import { inputLoginFormStyles } from "../../../../styles/formStyles";
import { Login } from "../../../../interfaces/interface";
import { ReactNode, useState } from "react";
import { IconEye, IconEyeOff } from "@tabler/icons-react";

interface PasswordInputProps {
  register: UseFormRegister<Login>;
  errors: FieldErrors<Login>;
}

const PasswordInput = ({ register, errors }: PasswordInputProps) => {
  const [showPassword, setShowPassword] = useState(false);
  return (
    <section className={inputLoginFormStyles.section}>
      <label htmlFor="password" className={inputLoginFormStyles.label}>
        PASSWORD
      </label>
      <div className="relative">
        <input
          id="password"
          type={showPassword ? "text" : "password"}
          className={inputLoginFormStyles.input}
          placeholder="Password"
          {...register("password")}
        />
        {errors.password && (
          <p className={inputLoginFormStyles.zodMessageLabel}>
            {errors.password.message as ReactNode}
          </p>
        )}

        <div
          className="absolute top-2 right-2 cursor-pointer"
          onClick={() => setShowPassword(!showPassword)}
          aria-label={
            showPassword ? "Ocultar contraseña" : "Mostrar contraseña"
          }
        >
          {showPassword ? (
            <IconEyeOff
              className={inputLoginFormStyles.iconEyeColors}
              size={24}
            />
          ) : (
            <IconEye
              className={inputLoginFormStyles.iconEyeColors}
              size={24}
            />
          )}
        </div>
      </div>
    </section>
  );
};

export default PasswordInput;
