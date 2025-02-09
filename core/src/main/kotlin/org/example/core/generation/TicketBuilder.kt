package org.example.core.generation

import org.example.core.Ticket

class TicketBuilder(val index: Int) {
    var numbersInTicketCount = 0
    private val columns = Array(9) { Column(it) }
    private val rows = Array(3) { arrayOfNulls<Int?>(9) }

    fun hasSpace(rangeIndex: Int): Boolean {
        val howManyInColumn = columns[rangeIndex].size
        return howManyInColumn < 3 && numbersInTicketCount < 15
    }

    fun place(value: Int, rangeIndex: Int) {
        numbersInTicketCount++
        columns[rangeIndex].add(value)
    }

    fun build(): Ticket {
        this.balanceEmptyCells()
        return Ticket(rows)
    }

    private fun balanceEmptyCells() {
        val rowsEmptyCells = Array(3) { RowEmptyCells(it) }

        for (column in columns.sortedBy { r -> r.size }) {
            if (column.size == 1) {
                val rowIndicesLeft = mutableListOf(0, 1, 2)
                val ordered = rowsEmptyCells.sortedByDescending { c -> c.count }
                val columnIndex = column.index

                (0..1).map {
                    val row = ordered[it]
                    rowIndicesLeft.remove(row.index)
                    row.count--
                }

                rows[rowIndicesLeft.first()][columnIndex] = column.first()
            }

            if (column.size == 2) {
                val columnIndicesLeft = mutableListOf(0, 1, 2)
                val row = rowsEmptyCells.maxByOrNull { c -> c.count }!!
                val columnIndex = column.index

                columnIndicesLeft.remove(row.index)
                row.count--

                val ordered = column.sortedBy { it }
                rows[columnIndicesLeft.first()][columnIndex] = ordered[0]
                rows[columnIndicesLeft.last()][columnIndex] = ordered[1]

            }

            if (column.size == 3) {
                val columnIndex = column.index
                val ordered = column.sortedBy { it }

                rows[0][columnIndex] = ordered[0]
                rows[1][columnIndex] = ordered[1]
                rows[2][columnIndex] = ordered[2]
            }
        }
    }

    private class Column(val index: Int) : ArrayList<Int?>(3)
    private class RowEmptyCells(val index: Int){
        var count: Int = 4
    }
}