
import { endpoints } from "../../../shared/api/endpoints";
import { http } from "../../../shared/api/https";

export const stockApi = {
  getBalance: (tenantId, productId) =>
    http.get(endpoints.stockBalance(tenantId, productId)),

  getMovements: (tenantId, productId) =>
    http.get(endpoints.stockMovements(tenantId, productId)),

  adjust: (tenantId, productId, data) =>
    http.post(endpoints.adjust(tenantId, productId), data),

  donate: (tenantId, productId, data) =>
    http.post(endpoints.donate(tenantId, productId), data),
};