import { endpoints } from "../../../shared/api/endpoints";
import { http } from "../../../shared/api/https";

export const ProductApi = {

  //  GET ALL
  getAll: async (tenantId) => {
    const res = await http.get(endpoints.products(tenantId));
    return res.data;
  },

  // GET BY ID
  getById: async (tenantId, id) => {
    const res = await http.get(`${endpoints.products(tenantId)}/${id}`);
    return res.data;
  },

  //  UPLOAD IMAGE
  uploadImage: async (file) => {
    const formData = new FormData();
    formData.append("file", file);

    //  مم: لا header
    const res = await http.post("/files/upload", formData);

    return res.data; // string: uploads/products/xxx.png
  },

  // ➕ CREATE PRODUCT
 create: async (tenantId, form) => {
    const fd = new FormData();
    fd.append("name", form.name ?? "");
    fd.append("description", form.description ?? "");
    fd.append("price", String(form.price ?? ""));
    fd.append("unit", form.unit ?? "");
    fd.append("categoryId", form.categoryId ?? "");
    if (form.imageFile) fd.append("image", form.imageFile);


    // for (const [k, v] of fd.entries()) console.log("FD", k, v);

    const res = await http.post(endpoints.products(tenantId), fd);
    return res.data;
  },
  //  UPDATE PRODUCT
update: async (tenantId, id, form) => {
  const formData = new FormData();

  formData.append("name", form.name);
  formData.append("description", form.description || "");
  formData.append("price", form.price);
  formData.append("unit", form.unit);
  formData.append("categoryId", form.categoryId);
  formData.append("isActive", true);

  if (form.imageFile) {
    formData.append("image", form.imageFile);
  }

  const res = await http.put(
    `${endpoints.products(tenantId)}/${id}`,
    formData
  );

  return res.data;
},

  //  DELETE
  remove: async (tenantId, id) => {
    const res = await http.delete(`${endpoints.products(tenantId)}/${id}`);
    return res.data;
  }
};