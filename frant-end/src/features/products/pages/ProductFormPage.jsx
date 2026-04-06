import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import ProductForm from "../components/ProductForm";
import { ProductApi } from "../api/products.api";
import { categorysApi } from "../../categories/api/categories.api";
import { emptyProductForm } from "../types/Product.type";

export default function ProductFormPage() {
  const { tenantId, id } = useParams();
  const navigate = useNavigate();

  const [form, setForm] = useState(emptyProductForm);
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    (async () => {
      const cats = await categorysApi.getAll(tenantId);
      setCategories(cats);

      if (id) {
        const p = await ProductApi.getById(tenantId, id);

        setForm({
          name: p.name,
          description: p.description,
          price: p.price,
          unit: p.unit,
          categoryId: p.categoryId,
          imageUrl: p.imageUrl,
          imageFile: null,
        });
      }
    })();
  }, [tenantId, id]);

  async function handleSubmit(data) {
    if (id) {
      await ProductApi.update(tenantId, id, data);
    } else {
      await ProductApi.create(tenantId, data);
    }

    navigate(`/tenants/${tenantId}/products`);
  }

  return (
    <div className="p-6">
      <ProductForm
        initialValues={form}
        categories={categories}
        onSubmit={handleSubmit}
        onCancel={() => navigate(`/tenants/${tenantId}/products`)}
        editingId={id}
      />
    </div>
  );
}