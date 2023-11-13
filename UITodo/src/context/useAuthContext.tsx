import { useContext } from "react";
import toast from "react-hot-toast";
import { AuthContext } from "./AuthenticationContext";

export const useAuthContext = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    toast.error(
      "Error inesperado con proveedor de Autenticación. Contacte con un administrador."
    );
    throw new Error("useAuth debe ser utilizado dentro de un AuthProvider");
  }
  return context;
};
