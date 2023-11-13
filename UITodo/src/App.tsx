import { RouterProvider } from "react-router-dom";
import { router } from "./router";
import { Toaster } from "react-hot-toast";
import { AuthProvider } from "./context/AuthenticationContext";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";



const App = () => {
  const queryClient = new QueryClient()
  return (
    <>
      <QueryClientProvider client={queryClient}>
        <AuthProvider>
          <Toaster />
          <RouterProvider router={router} />
        </AuthProvider>
      </QueryClientProvider>
    </>
  );
};
export default App;
