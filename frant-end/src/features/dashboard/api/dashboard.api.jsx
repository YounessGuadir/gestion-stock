import { endpoints } from "../../../shared/api/endpoints";
import { http } from "../../../shared/api/https";

export const dashboardApi = {
  get: (tenantId) =>
    http.get(endpoints.dashboard(tenantId))
};