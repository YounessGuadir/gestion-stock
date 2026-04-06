import { endpoints } from "../../../shared/api/endpoints"
import { http } from "../../../shared/api/https";

export const categorysApi = {
    getAll: async (tenantId) => {
        const res = await http.get(endpoints.categories(tenantId))
        return res.data;
    },

    getById: async (tenantId, categoryId) => {

        const res = await http.get(`${endpoints.categories(tenantId)}/${categoryId}`);
        return res.data;
    },

    create : async (tenantId, payload) => {
        const res = await http.post(endpoints.categories(tenantId), payload);
        return res.data;
    },
    update : async (tenantId, categoryId, payload) => {
        const res = await http.put(`${endpoints.categories(tenantId)}/${categoryId}`, payload);
        return res.data;
    },
    remove : async (tenantId, categoryId) => {

        const res = await http.delete(`${endpoints.categories(tenantId)}/${categoryId}`);
        return res.data;
    }
}