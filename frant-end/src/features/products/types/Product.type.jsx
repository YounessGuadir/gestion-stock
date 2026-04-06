export const emptyProductForm = {

    name: "",
    description: "",
    price: "",
    unit: "pcs",
    categoryId: "",
    imageUrl: null,

}

export function mapProductToFprmPayload(form) {
    return {
        name: form.name?.trim(),
        description: form.description?.trim() || null,
        price: Number(form.price),
        unit: form.unit?.trim() || "pcs",
        categoryId: form.categoryId,
        imageUrl: form.imageUrl?.trim() || null,
    }
}