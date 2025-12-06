<template>
  <div class="min-h-screen bg-background text-foreground">
    <!-- Header -->
    <header class="border-b border-border bg-card/50 backdrop-blur-sm sticky top-0 z-50">
      <div class="max-w-7xl mx-auto px-4 py-6">
        <div class="flex items-center gap-3">
          <div class="p-2 bg-accent/10 rounded-lg">
            <svg class="w-6 h-6 text-accent" fill="currentColor" viewBox="0 0 24 24">
              <path d="M20 3H4v10c0 2.21 1.79 4 4 4h6c2.21 0 4-1.79 4-4v-3h2c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm0 5h-2V5h2v3zM4 19h16v2H4z" />
            </svg>
          </div>
          <h1 class="text-3xl font-bold">Coffee Machine</h1>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="max-w-7xl mx-auto px-4 py-12">
      <div class="flex flex-col lg:flex-row gap-8">
        <!-- Coffee Grid -->
        <div class="flex-1">
          <h2 class="text-2xl font-bold mb-6">Available Coffees</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <CoffeeCard
              v-for="coffee in coffeeOptions"
              :key="coffee.id"
              :coffee="coffee"
              :on-add-to-cart="addToCart"
              :on-update-quantity="updateQuantity"
              :cart-item="cart.find((item) => item.id === coffee.id)"
            />
          </div>
        </div>

        <!-- Order Panel -->
        <OrderPanel
          :cart="cart"
          :total-cost="totalCost"
          :paid-amount="paidAmount"
          :change="change"
          :can-pay="canPay"
          :payment-denominations="paymentDenominations"
          :on-add-payment="addPayment"
          :on-pay="handlePay"
          :on-update-quantity="updateQuantity"
        />
      </div>
    </main>

    <!-- Error Modal -->
    <div v-if="showError" class="fixed inset-0 flex items-center justify-center bg-black/50 backdrop-blur-sm z-50">
      <div class="bg-card rounded-lg p-8 max-w-md w-full mx-4 border border-red-500/30">
        <div class="flex justify-center mb-4">
          <div class="p-3 bg-red-500/10 rounded-full">
            <svg class="w-8 h-8 text-red-500" fill="currentColor" viewBox="0 0 24 24">
              <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z" />
            </svg>
          </div>
        </div>
        <h3 class="text-2xl font-bold text-center mb-2 text-red-500">Error</h3>
        <p class="text-muted-foreground text-center mb-6">{{ errorMessage }}</p>
        <button
          @click="closeError"
          class="w-full rounded-lg bg-red-500 text-white font-bold py-3 hover:bg-red-600 transition-colors"
        >
          Try Again
        </button>
      </div>
    </div>

    <!-- Success Modal -->
    <div v-if="showSuccess" class="fixed inset-0 flex items-center justify-center bg-black/50 backdrop-blur-sm z-50">
      <div class="bg-card rounded-lg p-8 max-w-md w-full mx-4 border border-border">
        <div class="flex justify-center mb-4">
          <div class="p-3 bg-accent/10 rounded-full">
            <svg class="w-8 h-8 text-accent" fill="currentColor" viewBox="0 0 24 24">
              <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41L9 16.17z" />
            </svg>
          </div>
        </div>
        <h3 class="text-2xl font-bold text-center mb-2">Payment Successful!</h3>
        <p class="text-muted-foreground text-center mb-6">Your order has been confirmed.</p>

        <div v-if="changeBreakdown.length > 0" class="bg-secondary/20 rounded-lg p-4 mb-6 border border-border/50">
          <p class="text-sm font-semibold mb-3">Change Breakdown:</p>
          <div class="space-y-2">
            <div v-for="item in changeBreakdown" :key="item.value" class="flex justify-between text-sm">
              <span class="text-muted-foreground">₡{{ item.value.toLocaleString() }} × {{ item.count }}</span>
              <span class="font-semibold">₡{{ (item.value * item.count).toLocaleString() }}</span>
            </div>
          </div>
        </div>

        <p class="text-center text-sm text-muted-foreground">Closing in {{ countdownSeconds }}s...</p>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, watch } from 'vue'
import CoffeeCard from './components/CoffeeCard.vue'
import OrderPanel from './components/OrderPanel.vue'
import { useCart } from './composables/useCart'

export default {
  name: 'App',
  components: {
    CoffeeCard,
    OrderPanel,
  },
  setup() {
    const {
      cart,
      paidAmount,
      showSuccess,
      showError,
      errorMessage,
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
      closeError,
    } = useCart()

    const countdownSeconds = ref(3)

    watch(showSuccess, (newVal) => {
      if (newVal) {
        countdownSeconds.value = 7
        const interval = setInterval(() => {
          countdownSeconds.value -= 1
          if (countdownSeconds.value <= 0) {
            clearInterval(interval)
          }
        }, 1000)
      }
    })

    return {
      cart,
      paidAmount,
      showSuccess,
      showError,
      errorMessage,
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
      closeError,
      countdownSeconds,
    }
  },
}
</script>