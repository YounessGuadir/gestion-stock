import axios from "axios";
import keycloak from "../../auth/keycloak/keycloak";

export const http = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
});

http.interceptors.request.use(async (config) => {
  if (keycloak?.authenticated) {

    await keycloak.updateToken(30);
    config.headers = config.headers || {};
    config.headers.Authorization = `Bearer ${keycloak.token}`;



  }
  
    console.log("REQ =>", config.method?.toUpperCase(), config.baseURL + config.url);
  console.log("AUTH HEADER =>", config.headers.Authorization?.slice(0, 20) + "...");
  return config;
});