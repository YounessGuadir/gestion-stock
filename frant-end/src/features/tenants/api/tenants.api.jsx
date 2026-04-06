import { endpoints } from "../../../shared/api/endpoints";
import { http } from "../../../shared/api/https";

export const tenantsApi = {
    getAll:async ()=> (await http.get(endpoints.tenants)).data,
}