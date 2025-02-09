package org.example.core.generation

import org.example.core.Strip
import kotlin.random.Random

class StripBuildDirector {
    fun make(): Strip {
            val numbers1 = (1..9).toMutableList()
            val numbers2 = (10..19).toMutableList()
            val numbers3 = (20..29).toMutableList()
            val numbers4 = (30..39).toMutableList()
            val numbers5 = (40..49).toMutableList()
            val numbers6 = (50..59).toMutableList()
            val numbers7 = (60..69).toMutableList()
            val numbers8 = (70..79).toMutableList()
            val numbers9 = (80..90).toMutableList()

            val all = arrayOf(
                numbers1,
                numbers2,
                numbers3,
                numbers4,
                numbers5,
                numbers6,
                numbers7,
                numbers8,
                numbers9
            )

        val stripBuilder = StripBuilder();

        for (ticket in stripBuilder.tickets){
            for (i in all.indices){
                getForColumn(ticket, all[i], i)
            }
        }

        for (i in all.indices){
            val list = all[i]
            do{
                val index = Random.nextInt(list.size)
                stripBuilder.distribute(list[index], i)
                list.removeAt(index)
            }while (list.size > 0)
        }

        return stripBuilder.build();
    }

    private fun getForColumn(ticket: TicketBuilder, range: MutableList<Int>, rangeIndex: Int) {
        val index = Random.nextInt(range.size)
        ticket.place(range[index], rangeIndex)
        range.removeAt(index);
    }
}