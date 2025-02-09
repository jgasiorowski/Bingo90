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
                val index = Random.nextInt(availableNumberGroups[groupIndex].size)
                ticket.place(availableNumberGroups[groupIndex][index], groupIndex)
                availableNumberGroups[groupIndex].removeAt(index)
            }
        }
    }

    private fun distributeBalanced() {
        for (i in availableNumberGroups.indices) {
            val list = availableNumberGroups[i]
            do {
                val index = Random.nextInt(list.size)
                this.distribute(list[index], i)
                list.removeAt(index)
            } while (list.size > 0)
        }
    }

    private fun createStripFromTicketBuilders(): Strip {
        return Strip(tickets.map { t -> t.build() }.toTypedArray())
    }

    private fun distribute(value: Int, rangeIndex: Int) {
        val min = tickets.minOfOrNull { t -> t.numbersCount }
        val a = mutableListOf<Int>()
        val b = mutableListOf<Int>()

        for (ticket in tickets) {
            if (ticket.numbersCount == min) {
                a.add(ticket.index)
            } else {
                b.add(ticket.index)
            }
        }

        a.shuffle()
        val ordered = a + b

        for (i in tickets.indices) {
            val ticket = tickets[ordered[i]]

            if (ticket.canAccept(value, rangeIndex)) {
                ticket.place(value, rangeIndex)
                return
            }
        }
    }
}