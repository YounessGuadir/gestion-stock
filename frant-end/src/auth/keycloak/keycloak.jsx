import Keycloak from "keycloak-js";

const keycloak = new Keycloak({
  url: "http://localhost:8080",
  realm: "gestionstock",
  clientId: "gestionstock-spa",
});

export default keycloak;