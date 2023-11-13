import { createBrowserRouter } from "react-router-dom";
import { PrincipalLayout } from "../layouts";
import { Home } from "../pages/home";
import { Register } from "../pages/register";
import Login from "../pages/login";
import ErrorPage from "../pages/error";
import { ProtectedLayout } from "../layouts/protected";
import TodoPage from "../pages/todo";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <PrincipalLayout/>,
    errorElement: <ErrorPage/>,
    children:[
        {
            index:true,
            element:<Home/>
        },
        {
            element:<Register/>,
            path: '/register'
        }
        ,
        {
            element:<Login/>,
            path: '/login'
        }
        ,
        {
            element:<ProtectedLayout/>,
            path: '/todo',
            children:[
              {
                element:<TodoPage/>,
                index:true
              }
            ]
        }
    ],
  },
]);
