import React, { createContext, useContext, useEffect, useMemo, useState } from "react";
import keycloak from "./keycloak";


const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [ready, setReady] = useState(false);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [token, setToken] = useState(undefined);
  const [username, setUsername] = useState(undefined);
  const [roles, setRoles] = useState([]);

  useEffect(() => {
    let intervalId;

    (async () => {
      const authenticated = await keycloak.init({
        onLoad: "login-required",
        pkceMethod: "S256",
        checkLoginIframe: false,
      });

      setIsAuthenticated(authenticated);
      setToken(keycloak.token);
      setUsername(keycloak.tokenParsed?.preferred_username);
      console.log("User roles:", keycloak.tokenParsed?.realm_access?.roles);

      const rr = keycloak.tokenParsed?.realm_access?.roles ?? [];
      setRoles(rr);

      setReady(true);

      intervalId = window.setInterval(async () => {
        try {
          const refreshed = await keycloak.updateToken(30);
          if (refreshed) {
            setToken(keycloak.token);
            const newRoles = keycloak.tokenParsed?.realm_access?.roles ?? [];
            setRoles(newRoles);
          }
        } catch (e) {
          keycloak.login();
        }
      }, 10000);
    })();

    return () => {
      if (intervalId) window.clearInterval(intervalId);
    };
  }, []);

  const value = useMemo(() => ({
    ready,
    isAuthenticated,
    token,
    username,
    roles,
    login: () => keycloak.login(),
    logout: () => keycloak.logout({ redirectUri: "http://localhost:3000" }),
    hasRole: (role) => roles.includes(role),
  }), [ready, isAuthenticated, token, username, roles]);

  if (!ready) return <div style={{ padding: 24 }}>Loading auth...</div>;

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}