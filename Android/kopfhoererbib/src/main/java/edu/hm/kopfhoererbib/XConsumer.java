package edu.hm.kopfhoererbib;

/**
 * Created by simon on 06.12.16.
 *
 * Wird benoetigt, da Consumer erst ab APILevel 24 verfuegbar ist.
 */

public interface XConsumer <T> {

    void accept (T sth);

}
