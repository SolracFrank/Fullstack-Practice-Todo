import { FieldErrors, UseFormRegister } from "react-hook-form";
import { inputRegisterFormStyles } from "../../../../styles/formStyles";
import { Register } from "../../../../interfaces/interface";
import { ReactNode, useState } from "react";
import { IconEye, IconEyeOff } from "@tabler/icons-react";

interface PasswordInputProps {
  register: UseFormRegister<Register>;
  errors: FieldErrors<Register>;
  isRepeat?: boolean;
}

const PasswordInput = ({
  register,
  errors,
  isRepeat = false,
}: PasswordInputProps) => {
  const [showPassword, setShowPassword] = useState(false);
  return (
    <section className={inputRegisterFormStyles.section}>
      <label
        htmlFor={isRepeat ? "confirmpassword" : "password"}
        className={inputRegisterFormStyles.label}
      >
        {isRepeat ? "REPEAT PASSWORD" : "PASSWORD"}
      </label>
      <div className="relative">
        <input
          id={isRepeat ? "confirmpassword" : "password"}
          type={showPassword ? "text" : "password"}
          className={inputRegisterFormStyles.input}
          placeholder={isRepeat ? "Confirm Password" : "Password"}
          {...register(isRepeat ? "confirmPassword" : "password")}
        />
        {isRepeat
          ? errors.confirmPassword && (
              <p className={inputRegisterFormStyles.zodMessageLabel}>
                {errors.confirmPassword.message as ReactNode}
              </p>
            )
          : errors.password && (
              <p className={inputRegisterFormStyles.zodMessageLabel}>
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
              className={inputRegisterFormStyles.iconEyeColors}
              size={24}
            />
          ) : (
            <IconEye
              className={inputRegisterFormStyles.iconEyeColors}
              size={24}
            />
          )}
        </div>
      </div>
    </section>
  );
};

export default PasswordInput;
