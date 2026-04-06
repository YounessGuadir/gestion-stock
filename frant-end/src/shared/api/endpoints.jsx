export const endpoints = {
    tenants: "/tenants",
    categories: (tenantId) => `/tenants/${tenantId}/categories`,
    products: (tenantId) => `/tenants/${tenantId}/products`,
    stockBalance: (tenantId, productId) => `/tenants/${tenantId}/products/${productId}/stock/balance`,
    stockMovements: (tenantId, productId) => `/tenants/${tenantId}/products/${productId}/stock/movements`,
    donate: (tenantId, productId) => `/tenants/${tenantId}/products/${productId}/stock/donate`,
    adjust: (tenantId, productId) => `/tenants/${tenantId}/products/${productId}/stock/adjust`,
    dashboard: (tenantId) => `/dashboard?tenantId=${tenantId}`,
}
