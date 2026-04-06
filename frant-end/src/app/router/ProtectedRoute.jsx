import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../auth/keycloak/AuthContext"; // 

export default function ProtectedRoute() {
  const { ready, isAuthenticated } = useAuth();

  if (!ready) return <div className="p-6">Loading auth...</div>;
  if (!isAuthenticated) return <Navigate to="/" replace />;

  return <Outlet />;
}