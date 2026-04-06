import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";



import { toast } from "sonner";

import { ProductApi } from "../../products/api/products.api";
import { stockApi } from "../api/stock.api";
import DonateStockList from "../components/DonateStockList";


export default function DonateStockPage() {
  const { tenantId } = useParams();

  const [products, setProducts] = useState([]);

  useEffect(() => {
    loadProducts();
  }, []);

  async function loadProducts() {
    try {
      const data = await ProductApi.getAll(tenantId);
      setProducts(data);
    } catch (err) {
      console.error(err);
    }
  }

  async function handleConfirmDonation(items) {
    try {
      for (const item of items) {
        await stockApi.donate(tenantId, item.productId, {
          quantity: item.quantity,
          createdBy: "admin",
          reason: "donation",
        });
      }

      toast.success("Don confirmé avec succès !");
      loadProducts(); // refresh stock
    } catch (err) {
      console.error(err);
      toast.error("Erreur donation");
    }
  }

  return (
    <DonateStockList
      products={products}
      onConfirm={handleConfirmDonation}
    />
  );
}