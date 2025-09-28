import { useEffect } from "react";
import { useAppDispatch } from "../../store/hooks";
import { initializeAuth } from "../../store/actions/authActions";

const AuthInitializer: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    // Initialize auth state from localStorage on app start
    dispatch(initializeAuth() as any);
  }, [dispatch]);

  return <>{children}</>;
};

export default AuthInitializer;
