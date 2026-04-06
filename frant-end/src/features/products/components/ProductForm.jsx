import { useEffect, useState } from "react";
import { ImageIcon } from "lucide-react";
import { emptyProductForm } from "../types/Product.type";

export default function ProductForm({
  initialValues,
  categories,
  onSubmit,
  saving = false,
  editingId = null,
}) {
  const API_BASE = "https://localhost:7132";

  const [form, setForm] = useState(emptyProductForm);
  const [preview, setPreview] = useState("");

  useEffect(() => {
    if (initialValues) {
      setForm({
        name: initialValues.name || "",
        description: initialValues.description || "",
        price: initialValues.price ?? "",
        unit: initialValues.unit || "",
        categoryId: initialValues.categoryId || "",
        imageUrl: initialValues.imageUrl || "",
        imageFile: null,
      });

      // preview = image from backend
      if (initialValues.imageUrl) {
        setPreview(`${API_BASE}${initialValues.imageUrl}`);
      }
    }
  }, [initialValues]);

  function handleChange(e) {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  function handleFileChange(e) {
    const file = e.target.files?.[0];
    if (!file) return;

    setForm((prev) => ({ ...prev, imageFile: file }));

    const localPreview = URL.createObjectURL(file);
    setPreview(localPreview);
  }

  function handleSubmit(e) {
    e.preventDefault();
    onSubmit(form);
  }

  return (
    <div className="min-h-screen bg-[#f7f1e7] p-6">
      <div className="max-w-5xl mx-auto">

        <h1 className="text-2xl font-extrabold text-[#8b4f2f] mb-6">
          {editingId ? "Mise à jour du produit" : "Créer un produit"}
        </h1>

        <form onSubmit={handleSubmit} className="flex gap-10">

          {/* LEFT */}
          <div className="flex-1 space-y-4 max-w-md">

            <div>
              <label className="text-sm text-[#8b4f2f]">Nom</label>
              <input
                name="name"
                value={form.name}
                onChange={handleChange}
                className="w-full px-4 py-3 rounded-xl border border-[#e7d5c4] bg-[#fffdf9]"
              />
            </div>

            <div>
              <label className="text-sm text-[#8b4f2f]">Description</label>
              <textarea
                name="description"
                value={form.description}
                onChange={handleChange}
                className="w-full px-4 py-3 rounded-xl border border-[#e7d5c4] bg-[#fffdf9] min-h-[100px]"
              />
            </div>

            <div>
              <label className="text-sm text-[#8b4f2f]">Catégorie</label>
              <select
                name="categoryId"
                value={form.categoryId}
                onChange={handleChange}
                className="w-full px-4 py-3 rounded-xl border border-[#e7d5c4] bg-[#fffdf9]"
              >
                <option value="">Choisir</option>
                {categories.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="text-sm text-[#8b4f2f]">
                Image / Prix Unitaire
              </label>

              <div className="flex gap-3 mt-2">

                <label className="cursor-pointer px-4 py-2 rounded-lg bg-[#f3e2cf] text-[#8b4f2f]">
                  Choisir un fichier
                  <input
                    type="file"
                    hidden
                    onChange={handleFileChange}
                  />
                </label>

                <input
                  type="number"
                  name="price"
                  value={form.price}
                  onChange={handleChange}
                  className="flex-1 px-4 py-2 rounded-lg border border-[#e7d5c4]"
                />
              </div>
            </div>
             <select
              name="unit"
              value={form.unit}
              onChange={handleChange}
              className="w-full px-4 py-3 rounded-xl border border-[#e7d5c4] bg-[#fffdf9]"
            >
              <option value="">Sélectionner l'unité</option>
              <option value="kg">kg</option>
              <option value="litre">litre</option>
              <option value="pièce">pièce</option>
              <option value="boîte">boîte</option>
            </select>

            <button
              type="submit"
              className="mt-4 px-5 py-2 rounded-lg bg-[#e58b8b] text-white hover:bg-[#d96b6b]"
            >
              {saving ? "..." : "Mettre à jour"}
            </button>
          </div>

          {/* RIGHT */}
          <div className="flex flex-col gap-6">

            {/* OLD IMAGE */}
            {initialValues?.imageUrl && (
              <div className="w-40 h-40 rounded-2xl border border-[#f0a8a8] overflow-hidden">
                <img
                  src={`${API_BASE}${initialValues.imageUrl}`}
                  className="w-full h-full object-cover"
                />
              </div>
            )}

            {/* NEW IMAGE */}
            <div className="w-40 h-40 border-2 border-dashed border-[#f0a8a8] rounded-2xl flex items-center justify-center bg-[#fffdf9]">
              {preview ? (
                <img
                  src={preview}
                  className="w-full h-full object-cover rounded-2xl"
                />
              ) : (
                <ImageIcon className="text-[#f0a8a8]" />
              )}
            </div>

          </div>
        </form>
      </div>
    </div>
  );
}