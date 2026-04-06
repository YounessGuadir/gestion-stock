import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { categorysApi } from "../api/categories.api";
import { useAuth } from "../../../auth/keycloak/AuthContext";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";

import { Pencil, Trash2 } from "lucide-react";
import { toast } from "sonner";

export default function CategoriesPage() {
  const { tenantId } = useParams();
  const { hasRole } = useAuth();

  const isAdmin = hasRole("Admin");

  const [categories, setCategories] = useState([]);
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState(null);

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  const [loading, setLoading] = useState(true);

  async function loadCategories() {
    try {
      const data = await categorysApi.getAll(tenantId);
      setCategories(Array.isArray(data) ? data : []);
    } catch (e) {
      console.error(e);
      toast.error("Erreur lors du chargement");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    if (tenantId) loadCategories();
  }, [tenantId]);

  const handleSubmit = async () => {
    if (!name.trim()) {
      toast.error("Le nom est obligatoire");
      return;
    }

    try {
      if (editing) {
        await categorysApi.update(tenantId, editing.id, { name, description });
        toast.success("Catégorie modifiée");
      } else {
        await categorysApi.create(tenantId, { name, description });
        toast.success("Catégorie ajoutée");
      }

      await loadCategories();
      closeDialog();
    } catch (e) {
      console.error(e);
      toast.error("Erreur lors de l'enregistrement");
    }
  };

  const handleDelete = async (id) => {
    try {
      await categorysApi.remove(tenantId, id);
      await loadCategories();
      toast.success("Catégorie supprimée");
    } catch (e) {
      console.error(e);
      toast.error("Erreur suppression");
    }
  };

  const openEdit = (c) => {
    setEditing(c);
    setName(c.name || "");
    setDescription(c.description || "");
    setOpen(true);
  };

  const closeDialog = () => {
    setOpen(false);
    setEditing(null);
    setName("");
    setDescription("");
  };

  if (loading) {
    return <div className="p-6 text-[#8b6f5a]">Chargement...</div>;
  }

  return (
    <div className="min-h-screen bg-[#f7f1e7] p-6">
      <div className="mx-auto max-w-6xl space-y-6">
        {isAdmin && (
          <Button
            onClick={() => setOpen(true)}
            className="bg-[#d96b43] hover:bg-[#c85d36] text-white rounded-xl px-6 py-3 shadow-sm"
          >
            Ajouter une catégorie
          </Button>
        )}

        <div className="space-y-5">
          {categories.map((c) => (
            <div
              key={c.id}
              className="bg-[#fffdf9] border border-[#e7d5c4] rounded-2xl px-6 py-7 flex items-center justify-between shadow-sm"
            >
              <div>
                <h3 className="font-bold text-3xl text-[#d96b43]">
                  {c.name}
                </h3>
                <p className="mt-1 text-lg text-[#8a7768]">
                  {c.description || "Sans description"}
                </p>
              </div>

              {isAdmin && (
                <div className="flex gap-3">
                  <button
                    onClick={() => openEdit(c)}
                    className="p-3 rounded-2xl bg-[#f3e2cf] text-[#b28a63] hover:bg-[#ead7c1] transition-colors"
                  >
                    <Pencil className="h-6 w-6" />
                  </button>

                  <button
                    onClick={() => handleDelete(c.id)}
                    className="p-3 rounded-2xl bg-[#f8dede] text-[#e25555] hover:bg-[#f4cfcf] transition-colors"
                  >
                    <Trash2 className="h-6 w-6" />
                  </button>
                </div>
              )}
            </div>
          ))}
        </div>
      </div>

      <Dialog
        open={open}
        onOpenChange={(v) => {
          if (!v) closeDialog();
          else setOpen(true);
        }}
      >
        <DialogContent className="bg-[#fffdf9] border border-[#e7d5c4] rounded-2xl">
          <DialogHeader>
            <DialogTitle className="text-[#8b4f2f] text-xl font-bold">
              {editing ? "Modifier la catégorie" : "Nouvelle catégorie"}
            </DialogTitle>
          </DialogHeader>

          <div className="space-y-4">
            <Input
              placeholder="Nom"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="border-[#e7d5c4] bg-white focus-visible:ring-[#d96b43]"
            />

            <Textarea
              placeholder="Description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="border-[#e7d5c4] bg-white min-h-[120px] focus-visible:ring-[#d96b43]"
            />

            <Button
              onClick={handleSubmit}
              className="w-full bg-[#d96b43] hover:bg-[#c85d36] text-white rounded-xl"
            >
              {editing ? "Modifier" : "Ajouter"}
            </Button>
          </div>
        </DialogContent>
      </Dialog>
    </div>
  );
}