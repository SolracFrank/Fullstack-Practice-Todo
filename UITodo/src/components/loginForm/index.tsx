import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import LoginSchema from "../../schemas/loginSchema";
import { Login } from "../../interfaces/interface";
import { useNavigate } from "react-router";
import { TextInput } from "../formInputs/loginFormInput/textInput";
import PasswordInput from "../formInputs/loginFormInput/passwordInput";
import { inputLoginFormStyles } from "../../styles/formStyles";
import useAuthenticate from "../../hooks/useAuthentication";

import toast from "react-hot-toast";
import { useAuthContext } from "../../context/useAuthContext";

const LoginForm = () => {
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Login>({
    resolver: zodResolver(LoginSchema),
  });

  const { loginUser } = useAuthenticate();
  const { login } = useAuthContext();

  async function onSubmit(data: Login) {
    setIsLoading(true);
    try {
      const response = await loginUser(data);
      if (
        response.status == 200 ||
        response.status == 201 ||
        response.status == 204
      ) {
        if (response.data.jwToken) {
          console.table(response.data);
          await login({
            userId: response.data.id,
            userName: response.data.userName,
            jwToken: response.data.jwToken,
            refreshToken: response.data.refreshToken,
            jwtExpires: response.data.jwtExpires,
            refreshTokenExpires: response.data.refreshTokenExpires,
            todoUser: response.data.userId,
          });
          navigate("/", { replace: true });
        } else {
          toast.error("Ocurrió un error, vuelva a intentarlo nuevamnte.");
        }
      } else if (response.status == 400) {
        toast.error("Hay un error en la petición.");
      }
    } catch (error) {
      toast.error(`Ocurrio un error en el login`);
    } finally {
      setIsLoading(false);
    }
  }
  return (
    <div className="block border-2 border-solid 
     border-gray-300 dark:border-black w-2/6 bg-gray-100 dark:bg-oscure-400 shadow-lg shadow-day-200 dark:shadow-oscure-400 
     rounded-3xl">
      <section className="my-4 w-full flex flex-wrap justify-center text-center">
        <h1 className="text-3xl text-oscure-400 dark:text-oscure-100 my-4">LOGIN</h1>
      </section>
      <section className="w-full px-12">
        <form
          action=""
          className="w-full p-6"
          onSubmit={handleSubmit(onSubmit)}
        >
          <div className="grid grid-cols-12 gap-4 gap-y-8">
            <TextInput
              errors={errors}
              register={register}
              fieldName="email"
              type="email"
            />
            <PasswordInput errors={errors} register={register} />
            <section className="col-span-12 flex mx-auto w-3/5 my-4">
              <input
                id="submit-register"
                title="submit register"
                disabled={isLoading}
                type="submit"
                value={isLoading ? "LOGGING IN" : "LOGIN"}
                className={`${inputLoginFormStyles.input} ${inputLoginFormStyles.button} `}
              />
            </section>
          </div>
        </form>
      </section>
    </div>
  );
};

export default LoginForm;
