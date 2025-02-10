package org.example.core.generation

import org.example.core.Ticket

class TicketBuilder(val index: Int) {
    var numbersCount = 0
    val ranges = Array(9) { Column(it) }
    val rows = arrayOf(
        arrayOfNulls<Int?>(9),
        arrayOfNulls<Int?>(9),
        arrayOfNulls<Int?>(9)
    )

    fun canAccept(value: Int, rangeIndex: Int): Boolean {
        val howManyInRange = ranges[rangeIndex].items.size
        return howManyInRange < 3 && numbersCount < 15
    }

    fun place(value: Int, rangeIndex: Int) {
        numbersCount++
        ranges[rangeIndex].items.add(value)
    }

    fun build() : Ticket {
        return Ticket(rows)
    }

    fun balanceEmptyCells() {
        val emptyCellsLeft = arrayOf(
            Pair(0, 4),
            Pair(1, 4),
            Pair(2, 4)
        )

        for (range in ranges.sortedBy { r -> r.items.size }){
            if (range.items.size == 1){
                val rowIndicesLeft = mutableListOf( 0, 1, 2)
                val ordo = emptyCellsLeft.sortedByDescending { c -> c.count }
                var row = ordo[0]
                val columnIndex = range.index
                rowIndicesLeft.remove(row.index)
                row.count--

                row = ordo[1]

                rowIndicesLeft.remove(row.index)
                row.count--

                rows[rowIndicesLeft.first()][columnIndex] = range.items.first()

            }

            if (range.items.size == 2){
                val rowIndicesLeft = mutableListOf( 0, 1, 2)
                val row = emptyCellsLeft.sortedByDescending { c -> c.count }[0]
                val columnIndex = range.index

                rowIndicesLeft.remove(row.index)
                row.count--

                val ordered = range.items.sortedBy { i -> i }
                rows[rowIndicesLeft.first()][columnIndex] = ordered[0]
                rows[rowIndicesLeft.last()][columnIndex] = ordered[1]

            }

            if (range.items.size == 3){
                val columnIndex = range.index
                val ordered = range.items.sortedBy{ i -> i }

                rows[0][columnIndex] = ordered[0]
                rows[1][columnIndex] = ordered[1]
                rows[2][columnIndex] = ordered[2]
            }
        }
    }

    class Pair(val index: Int, var count: Int)

    class Column(val index: Int) {
        val items = mutableListOf<Int?>()
    }
}