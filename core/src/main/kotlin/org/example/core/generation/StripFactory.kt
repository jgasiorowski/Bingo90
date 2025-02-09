package org.example.core.generation

import org.example.core.Strip
import kotlin.random.Random

class StripFactory {
    private val tickets = Array(6) { TicketBuilder(it) }
    private val availableNumberGroups = NumberGroupsProvider.provide()

    fun create() : Strip {
        this.distributeEqually()
        this.distributeBalanced()

        return this.createStripFromTicketBuilders()
    }

    private fun distributeEqually() {
        for (ticket in tickets) {
            for (groupIndex in availableNumberGroups.indices) {
                val randomNumberIndex = Random.nextInt(availableNumberGroups[groupIndex].size)
                ticket.place(availableNumberGroups[groupIndex][randomNumberIndex], groupIndex)
                availableNumberGroups[groupIndex].removeAt(randomNumberIndex)
            }
        }
    }

    private fun distributeBalanced() {
        for (groupIndex in availableNumberGroups.indices) {
            val group = availableNumberGroups[groupIndex]
            do {
                val randomNumberIndex = Random.nextInt(group.size)
                this.distribute(group[randomNumberIndex], groupIndex)
                group.removeAt(randomNumberIndex)
            } while (group.size > 0)
        }
    }

    private fun createStripFromTicketBuilders(): Strip {
        return Strip(tickets.map { t -> t.build() }.toTypedArray())
    }

    private fun distribute(value: Int, rangeIndex: Int) {
        val minimalNumbersCountInTickets = tickets.minOfOrNull { t -> t.numbersInTicketCount }
        val ticketsWithMinimalNumbersCount = mutableListOf<Int>()
        val otherTickets = mutableListOf<Int>()

        for (ticket in tickets) {
            if (ticket.numbersInTicketCount == minimalNumbersCountInTickets) {
                ticketsWithMinimalNumbersCount.add(ticket.index)
            } else {
                otherTickets.add(ticket.index)
            }
        }

        ticketsWithMinimalNumbersCount.shuffle()
        val ordered = ticketsWithMinimalNumbersCount + otherTickets

        for (i in tickets.indices) {
            val ticket = tickets[ordered[i]]

            if (ticket.hasSpace(rangeIndex)) {
                ticket.place(value, rangeIndex)
                return
            }
        }
    }
}