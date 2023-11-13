import {  useState } from "react";
import imgRegister from "../../assets/images/register-image.webp";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import RegisterSchema from "../../schemas/registerSchema";
import { ProblemDetails, Register } from "../../interfaces/interface";
import { inputRegisterFormStyles } from "../../styles/formStyles";
import useRegister from "../../hooks/useRegister";
import { AxiosError } from "axios";
import { useNavigate } from "react-router-dom";
import { toast } from "react-hot-toast";
import PasswordInput from "../formInputs/registerFormInput/passwordInput";
import { TextInput } from "../formInputs/registerFormInput/textInput";

const RegisterForm = () => {
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Register>({
    resolver: zodResolver(RegisterSchema),
  });

  const { registerUser } = useRegister();

  async function onSubmit(data: Register) {
    setIsLoading(true);
    try {
      const response = await registerUser(data);
      if (response.status === 200 || response.status === 201) {
        navigate("/login");
      }
    } catch (error) {
      const axiosError = error as AxiosError;
      const problemDetails = axiosError.response?.data as ProblemDetails;
      const { title, detail } = problemDetails || {};
      toast.error(`${title} ${detail}`);
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div
      className="flex border-2 border-solid rounded-xl border-gray-300 dark:border-oscure-300 w-3/5 bg-teal-50 dark:bg-gray-200
            shadow-lg shadow-teal-100 dark:shadow-oscure-400"
    >
      <section className="w-1/2 relative">
        <img
          className="absolute top-0 left-0 h-full w-full object-cover"
          src={imgRegister}
          alt="register-logo"
          draggable="false"
        />
      </section>

      <section className="w-1/2">
        <form
          action=""
          className="w-full p-6"
          onSubmit={handleSubmit(onSubmit)}
        >
          <div className="flex justify-center mb-5">
            <h1 className="text-4xl font-semibold text-center text-black">
              Register
            </h1>
          </div>

          <div className="grid grid-cols-12 gap-4 gap-y-8 relative">
            <TextInput errors={errors} register={register} fieldName="name" />
            <TextInput
              errors={errors}
              register={register}
              fieldName="lastName"
            />
            <TextInput
              errors={errors}
              register={register}
              fieldName="username"
            />
            <TextInput
              errors={errors}
              register={register}
              fieldName="email"
              type="email"
            />
            <PasswordInput errors={errors} register={register} />
            <PasswordInput
              errors={errors}
              register={register}
              isRepeat={true}
            />
            <section className="col-span-12 flex mx-auto w-3/5 my-4">
              <input
                id="submit-register"
                title="submit register"
                disabled={isLoading}
                type="submit"
                value={isLoading ? "SIGNING" : "SIGN UP"}
                className={`${inputRegisterFormStyles.input} ${inputRegisterFormStyles.button} `}
              />
            </section>
          </div>
        </form>
      </section>
    </div>
  );
};

export default RegisterForm;
