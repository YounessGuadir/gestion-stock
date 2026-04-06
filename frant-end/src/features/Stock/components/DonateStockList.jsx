import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { toast } from "sonner";
import { Plus, Trash2, Search, Package } from "lucide-react";

const API_BASE = "https://localhost:7132";

function getImageUrl(path) {
  if (!path) return null;
  const clean = path.startsWith("/") ? path.slice(1) : path;
  return `${API_BASE}/${clean}`;
}

export default function DonateStockList({ products, onConfirm }) {
  const [search, setSearch] = useState("");
  const [items, setItems] = useState([]);

  const filtered = products.filter(p =>
    p.name.toLowerCase().includes(search.toLowerCase())
  );

  function add(product) {
    if (items.find(i => i.productId === product.id)) {
      toast.error("Produit déjà ajouté");
      return;
    }

    setItems([...items, { productId: product.id, quantity: 1 }]);
    toast.success("Produit ajouté au don");
  }

  function remove(productId) {
    setItems(items.filter(i => i.productId !== productId));
    toast.info("Produit retiré");
  }

  function update(productId, qty) {
    setItems(items.map(i =>
      i.productId === productId
        ? { ...i, quantity: Math.max(1, qty) }
        : i
    ));
  }

  function confirm() {
    if (!items.length) {
      toast.error("Aucun produit ajouté");
      return;
    }

    onConfirm(items);
    setItems([]);
  }

  return (
    <div>
      <h1 className="text-2xl font-extrabold text-[#3b2f2f] mb-6">Donner</h1>

      <div className="grid grid-cols-1 lg:grid-cols-[320px_1fr] gap-6">

        {/* LEFT */}
        <div className="space-y-3">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-[#8a7768]" />
            <Input
              placeholder="Rechercher..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="pl-9"
            />
          </div>

          <div className="space-y-3 max-h-[500px] overflow-y-auto pr-1">
            {filtered.map(p => (
              <div
                key={p.id}
                className=" rounded-xl border border-[#e6dcd2] p-4 flex gap-3 shadow-sm hover:shadow-md transition"
              >
                {/* IMAGE */}
                <div className="w-16 h-16 rounded-xl  flex items-center justify-center overflow-hidden">
                  {p.imageUrl ? (
                    <img
                      src={getImageUrl(p.imageUrl)}
                      alt={p.name}
                      className="w-full h-full object-cover"
                    />
                  ) : (
                    <Package className="h-6 w-6 text-[#8a7768]" />
                  )}
                </div>

                {/* INFO */}
                <div className="flex-1">
                  <h3 className="font-semibold text-[#3b2f2f]">{p.name}</h3>

                  <p className="text-sm text-[#c96c4b] font-bold mt-1">
                    {p.quantity} {p.unit}
                  </p>

                  <button
                    onClick={() => add(p)}
                    className="mt-2 w-7 h-7 rounded-full bg-[#c96c4b] text-white flex items-center justify-center hover:opacity-80"
                  >
                    <Plus size={14} />
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* RIGHT */}
        <div className=" rounded-xl border border-[#e6dcd2] min-h-[300px] overflow-hidden">

          {!items.length ? (
            <p className="p-6 text-center text-[#8a7768]">
              Ajouter des produits
            </p>
          ) : (
            <table className="w-full text-sm">
              <thead>
                <tr className="border-b border-[#eee] text-[#8a7768]">
                  <th className="p-3 text-left">Image</th>
                  <th className="p-3 text-left">Nom</th>
                  <th className="p-3 text-left">Quantité</th>
                  <th className="p-3 text-left">Unité</th>
                  <th className="p-3 text-left">Action</th>
                </tr>
              </thead>

              <tbody>
                {items.map(item => {
                  const p = products.find(x => x.id === item.productId);

                  return (
                    <tr key={item.productId} className="border-b last:border-0">
                      {/* IMAGE */}
                      <td className="p-3">
                        {p?.imageUrl ? (
                          <img
                            src={getImageUrl(p.imageUrl)}
                            className="w-10 h-10 rounded object-cover"
                          />
                        ) : (
                          <div className="w-10 h-10 bg-[#f3ede7] rounded flex items-center justify-center">
                            <Package size={14} />
                          </div>
                        )}
                      </td>

                      <td className="p-3 font-semibold">{p?.name}</td>

                      <td className="p-3">
                        <Input
                          type="number"
                          min={1}
                          value={item.quantity}
                          onChange={(e) =>
                            update(item.productId, Number(e.target.value))
                          }
                          className="w-20 h-8"
                        />
                      </td>

                      <td className="p-3 text-[#8a7768]">{p?.unit}</td>

                      <td className="p-3">
                        <button
                          onClick={() => remove(item.productId)}
                          className="p-2 rounded-lg bg-red-100 text-red-500 hover:bg-red-200"
                        >
                          <Trash2 size={14} />
                        </button>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          )}

          {items.length > 0 && (
            <div className="p-4">
              <Button
                onClick={confirm}
                className="bg-[#6b3e2e] hover:bg-[#5a3326] text-white"
              >
                Confirmer le Don
              </Button>
            </div>
          )}
        </div>

      </div>
    </div>
  );
}