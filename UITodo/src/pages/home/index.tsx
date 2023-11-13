import { useAuthContext } from "../../context/useAuthContext";
import AuthHome from "../../views/home/authHomeView";
import DefaultHome from "../../views/home/defaultHomeView";

export const Home = () => {

  const { isAuthenticated } = useAuthContext();
  return (
    <div className="w-full h-full">
      {isAuthenticated ? (
       <AuthHome/>
      ) : (
        <DefaultHome/>
      )}
    </div>
  );
};
