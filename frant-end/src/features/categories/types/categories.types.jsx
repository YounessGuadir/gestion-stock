export const emptyCategoryForm = {
    name: "",
    description: "",
}

export function mapCategoryToForm(category) {
    return {
        name: category?.name || "",
        description: category?.description || "",
    }
}
