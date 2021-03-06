import React, { useEffect } from "react";
import { useParams, Redirect } from "react-router-dom";
import { toast } from "react-toastify";
import { Employee, Employer } from "../components/login";
import auth from "../core/authService";

export function Login(prop) {
  const { role } = useParams();

  useEffect(()=>{
    document.getElementById("root").scrollIntoView();

  },[]);
  
  if (auth.getCurrentUser()) {
    return <Redirect to="/" />;
  }

  if (role === "Employee") {
    return (
      <>
        <Employee prop={prop} />
      </>
    );
  } else {
    return (
      <>
        <Employer prop={prop} />
      </>
    );
  }
}
