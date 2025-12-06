import { ref, computed } from 'vue'

export function useCart() {
  const cart = ref([])
  const paidAmount = ref(0)
  const showSuccess = ref(false)
  const changeBreakdown = ref([])

  const coffeeOptions = [
    { id: "americano", name: "Americano", price: 950, stock: 12 },
    { id: "cappuccino", name: "Cappuccino", price: 1200, stock: 0 },
    { id: "latte", name: "Latte", price: 1400, stock: 8 },
    { id: "mocaccino", name: "Mocaccino", price: 1500, stock: 15 },
  ]

  const paymentDenominations = [
    { value: 1000, label: "1000", type: "bill" },
    { value: 500, label: "500", type: "coin" },
    { value: 100, label: "100", type: "coin" },
    { value: 50, label: "50", type: "coin" },
    { value: 25, label: "25", type: "coin" },
  ]

  const totalCost = computed(() => {
    return cart.value.reduce((sum, item) => sum + item.price * item.quantity, 0)
  })

  const change = computed(() => paidAmount.value - totalCost.value)
  const canPay = computed(() => paidAmount.value >= totalCost.value && totalCost.value > 0)

  const addToCart = (coffee) => {
    const existing = cart.value.find((item) => item.id === coffee.id)
    if (existing) {
      existing.quantity += 1
    } else {
      cart.value.push({ ...coffee, quantity: 1 })
    }
  }

  const updateQuantity = (id, quantity) => {
    if (quantity <= 0) {
      cart.value = cart.value.filter((item) => item.id !== id)
    } else {
      const item = cart.value.find((item) => item.id === id)
      if (item) {
        item.quantity = quantity
      }
    }
  }

  const addPayment = (amount) => {
    paidAmount.value += amount
  }

  const calculateChange = () => {
    if (change.value < 0) return []

    let remaining = change.value
    const denominations = [1000, 500, 100, 50, 25].sort((a, b) => b - a)
    const breakdown = []

    denominations.forEach((denom) => {
      if (remaining >= denom) {
        const count = Math.floor(remaining / denom)
        breakdown.push({ value: denom, count })
        remaining -= count * denom
      }
    })

    return breakdown
  }

  const handlePay = () => {
    if (canPay.value) {
      const breakdown = calculateChange()
      changeBreakdown.value = breakdown
      showSuccess.value = true

      setTimeout(() => {
        showSuccess.value = false
        cart.value = []
        paidAmount.value = 0
        changeBreakdown.value = []
      }, 3000)
    }
  }

  return {
    cart,
    paidAmount,
    showSuccess,
    changeBreakdown,
    coffeeOptions,
    paymentDenominations,
    totalCost,
    change,
    canPay,
    addToCart,
    updateQuantity,
    addPayment,
    handlePay,
  }
}