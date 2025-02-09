package org.example.core.generation

import org.example.core.extensions.columns
import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.function.Executable
import org.junit.jupiter.params.ParameterizedTest
import org.junit.jupiter.params.provider.ValueSource
import kotlin.test.Test
import kotlin.time.DurationUnit
import kotlin.time.measureTime
import kotlin.time.toDuration

class StripBuildDirectorTest {
    private val strip = StripBuildDirector().make()

    @Test
    fun `Strip contains 6 tickets`() {
        assertEquals(6, strip.tickets.size)
    }

    @Test
    fun `A bingo ticket consists of 9 columns`() {
        assertAll(
            strip.tickets.map { ticket ->
                Executable { assertEquals(9, ticket.columns.size) }
            }
        )
    }

    @Test
    fun `A bingo ticket consists of 3 rows`() {
        assertAll(
            strip.tickets.map { ticket ->
                Executable { assertEquals(3, ticket.rows.size) }
            }
        )
    }

    @Test
    fun `Each ticket row contains five numbers and four blank spaces`() {
        assertAll(
            strip.tickets.map { ticket ->
                ticket.rows.map { row ->
                    Executable { assertEquals(5, row.filterNotNull().size) }
                }
            }.flatten()
        )
    }

    @Test
    fun `Each ticket column consists of one, two or three numbers and never three blanks`() {
        assertAll(
            strip.tickets.map { ticket ->
                ticket.columns.map { column ->
                    Executable { assertTrue(column.filterNotNull().size in 1..3) }
                }
            }.flatten()
        )
    }

    @Test
    fun `The first column contains numbers from 1 to 9 (only nine)`() {
        assertAll(
            strip.tickets.map { ticket ->
                Executable { assertTrue(ticket.columns[0].filterNotNull().all { it in 1..9 }) }
            }
        )
    }

    @ParameterizedTest
    @ValueSource(ints = [1, 2, 3, 4, 5, 7])
    fun `The second column numbers from 10 to 19 (ten), the third, 20 to 29 and so on (until last)`(columnIndex: Int) {
        val rangeStart = columnIndex * 10
        val rangeEnd = rangeStart + 9

        val expectedRangeInColumn = rangeStart..rangeEnd

        assertAll(
            strip.tickets.map { ticket ->
                Executable {
                    assertTrue(
                        ticket.columns[columnIndex].filterNotNull().all { it in expectedRangeInColumn })
                }
            }
        )
    }

    @Test
    fun `The last column contains numbers from 80 to 90 (eleven)`() {
        assertAll(
            strip.tickets.map { ticket ->
                Executable { assertTrue(ticket.columns[8].filterNotNull().all { it in 80..90 }) }
            }
        )
    }

    @Test
    fun `Numbers in the ticket columns are ordered from top to bottom (ASC)`() {
        assertAll(
            strip.tickets.map { ticket ->
                ticket.columns.map { column ->
                    Executable {
                        assertTrue(
                            column
                                .filterNotNull()
                                .zipWithNext()
                                .all { (prev, current) -> prev <= current }
                        )
                    }
                }
            }.flatten()
        )
    }

    @Test
    fun `There can be no duplicate numbers between 1 and 90 in the strip`() {
        val allNumbers = strip.tickets.map { ticket ->
            ticket.rows.map { row ->
                row.filterNotNull().map { it }
            }.flatten()
        }.flatten()

        assertAll(
            { assertEquals(allNumbers.size, allNumbers.toSet().size) },
            { assertTrue(allNumbers.all { it in 1..90 }) }
        )
    }

    @Test
    fun `Every number from 1 to 90 to appear across all 6 tickets`() {
        val allPossibleNumbers = (1..90).toMutableList()
        val allNumbers = strip.tickets.map { ticket ->
            ticket.rows.map { row ->
                row.filterNotNull().map { allPossibleNumbers.remove(it) }
            }
        }

        assertEquals(0, allPossibleNumbers.size)
    }

    @Test
    fun `Generate 10k strips in less than 1s`() {
        val timeTaken = measureTime {
            for (i in (1..10000)) {
                StripBuildDirector().make();
            }
        }

        println("10k strips generated in $timeTaken")
        assertTrue(timeTaken < 1.toDuration(DurationUnit.SECONDS))
    }
}