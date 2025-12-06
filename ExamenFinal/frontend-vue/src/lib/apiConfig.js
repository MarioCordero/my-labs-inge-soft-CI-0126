const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5011'

export const API_ENDPOINTS = {
  COFFEE: {
    GET_ALL: `${API_BASE_URL}/getCoffees`,
    BUY: `${API_BASE_URL}/buyCoffee`,
    GET_DENOMINATIONS: `${API_BASE_URL}/getPaymentDenominations`,
  },
}

export default API_ENDPOINTS