import { ref, computed, onMounted } from 'vue'
import API_ENDPOINTS from '../lib/apiConfig'

export function useCart() {
  const cart = ref([])
  const paidAmount = ref(0)
  const showSuccess = ref(false)
  const showError = ref(false)
  const errorMessage = ref('')
  const changeBreakdown = ref([])
  const changeAmount = ref(0)
  const coffeeOptions = ref([])
  const isLoading = ref(false)

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

  const fetchCoffees = async () => {
    try {
      isLoading.value = true
      const response = await fetch(API_ENDPOINTS.COFFEE.GET_ALL)
      const data = await response.json()
      coffeeOptions.value = data.map((coffee) => ({
        id: coffee.name.toLowerCase(),
        name: coffee.name,
        price: coffee.priceInCents,
        stock: coffee.stock
      }))
    } catch (err) {
      console.error('Failed to fetch coffees:', err)
      errorMessage.value = 'Failed to load coffees'
      showError.value = true
    } finally {
      isLoading.value = false
    }
  }

  onMounted(() => {
    fetchCoffees()
  })

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

  const buildOrderRequest = () => {
    const order = {}
    cart.value.forEach((item) => {
      order[item.name] = item.quantity
    })

    const coins = []
    const bills = []
    let remaining = paidAmount.value
    const denominations = [1000, 500, 100, 50, 25].sort((a, b) => b - a)

    denominations.forEach((denom) => {
      while (remaining >= denom) {
        if (denom >= 1000) {
          bills.push(denom)
        } else {
          coins.push(denom)
        }
        remaining -= denom
      }
    })

    return {
      order,
      totalPayment: paidAmount.value,
      payment: {
        coins: coins,
        bills: bills,
      }
    }
  }

  const parseChangeBreakdown = (changeBreakdown) => {
    return Object.entries(changeBreakdown).map(([value, count]) => ({
      value: parseInt(value),
      count: count
    }))
  }

  const closeError = () => {
    showError.value = false
    errorMessage.value = ''
  }

  const handlePay = async () => {
    if (!canPay.value) return

    try {
      isLoading.value = true
      closeError()
      
      const orderRequest = buildOrderRequest()
      
      const response = await fetch(API_ENDPOINTS.COFFEE.BUY, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(orderRequest),
      })

      const result = await response.json()

      if (response.ok && result.code === 0) {
        // Success
        changeAmount.value = result.changeAmount || 0
        changeBreakdown.value = result.changeBreakdown ? parseChangeBreakdown(result.changeBreakdown) : []
        showSuccess.value = true

        setTimeout(() => {
          showSuccess.value = false
          cart.value = []
          paidAmount.value = 0
          changeBreakdown.value = []
          changeAmount.value = 0
          // Refetch coffees after purchase
          fetchCoffees()
        }, 3000)
      } else {
        // Error from backend
        errorMessage.value = result.message || 'Payment failed'
        showError.value = true
      }
    } catch (err) {
      console.error('Failed to process payment:', err)
      errorMessage.value = 'Failed to process payment. Please try again.'
      showError.value = true
    } finally {
      isLoading.value = false
    }
  }

  return {
    cart,
    paidAmount,
    showSuccess,
    showError,
    errorMessage,
    changeBreakdown,
    changeAmount,
    coffeeOptions,
    paymentDenominations,
    totalCost,
    change,
    canPay,
    isLoading,
    addToCart,
    updateQuantity,
    addPayment,
    handlePay,
    closeError,
  }
}